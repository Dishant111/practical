```markdown
# Setting up a .NET API with Caddy and Docker Compose for Automatic HTTPS

This document provides a comprehensive guide to deploying your .NET API using Docker Compose, with Caddy acting as a reverse proxy for automatic HTTPS. This setup is highly recommended for production environments due to its simplicity, security, and efficiency in managing SSL certificates.

## Table of Contents

1.  [Introduction](#1-introduction)
2.  [Prerequisites](#2-prerequisites)
3.  [Project Structure](#3-project-structure)
4.  [Step 1: Adjust Your .NET API's Dockerfile](#4-step-1-adjust-your-net-apis-dockerfile)
5.  [Step 2: Create the Caddyfile](#5-step-2-create-the-caddyfile)
6.  [Step 3: Create the docker-compose.yml](#6-step-3-create-the-docker-composeyml)
7.  [How to Run Your Setup](#7-how-to-run-your-setup)
8.  [Accessing Your API](#8-accessing-your-api)
9.  [Stopping and Removing Services](#9-stopping-and-removing-services)
10. [Key Concepts and Notes](#10-key-concepts-and-notes)

---

## 1. Introduction

Deploying applications with Docker offers consistency and portability. When it comes to web APIs, securing communication with HTTPS is paramount. Caddy is an excellent choice for this, as it automates the process of obtaining and renewing SSL certificates from Let's Encrypt. By combining your .NET API, Caddy, and Docker Compose, you get a robust, secure, and easily manageable deployment.

This setup ensures:

* **Automatic HTTPS**: Caddy handles SSL certificate provisioning and renewal.
* **Simplified Deployment**: Docker Compose orchestrates both your API and Caddy.
* **Security**: Your API container doesn't directly handle SSL keys.
* **Efficiency**: Dedicated reverse proxy for optimized traffic handling.

---

## 2. Prerequisites

Before you begin, ensure you have the following installed on your system:

* **Docker Desktop** (for Windows/macOS) or **Docker Engine** (for Linux): This includes Docker Compose.
* Your **.NET API project** (e.g., `WebApplication1`).
* A **domain name** (for production deployment with real HTTPS certificates). For local testing, `localhost` will suffice.

---

## 3. Project Structure

For this guide, we'll assume a project structure similar to this:

```
your-solution-root/
├── WebApplication1/
│   ├── WebApplication1.csproj
│   ├── Dockerfile             <-- Your API's Dockerfile (will be modified)
│   └── ... (other API files)
├── Caddyfile                  <-- New file for Caddy configuration
└── docker-compose.yml         <-- New file for Docker Compose orchestration
```

---

## 4. Step 1: Adjust Your .NET API's Dockerfile

Your .NET API container will run internally on an HTTP port (e.g., `80`). Caddy will then proxy external HTTPS requests to this internal HTTP endpoint.

**Locate your API's `Dockerfile` (e.g., `WebApplication1/Dockerfile`) and update its content as follows:**

```dockerfile
# See [https://aka.ms/customizecontainer](https://aka.ms/customizecontainer) to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM [mcr.microsoft.com/dotnet/aspnet:8.0](https://mcr.microsoft.com/dotnet/aspnet:8.0) AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80 # Changed to 80 for Caddy to proxy to
# EXPOSE 8081 # You can remove this if not needed internally

# This stage is used to build the service project
FROM [mcr.microsoft.com/dotnet/sdk:8.0](https://mcr.microsoft.com/dotnet/sdk:8.0) AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApplication1/WebApplication1.csproj", "WebApplication1/"]
RUN dotnet restore "./WebApplication1/WebApplication1.csproj" --disable-parallel

# Copy the rest of the application files
COPY . .
WORKDIR "/src/WebApplication1"

# Build the application
RUN dotnet build "./WebApplication1.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApplication1.csproj" -c $BUILD_CONFIGURATION --no-build -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Set ASPNETCORE_URLS to ensure the app listens on port 80 inside the container
ENV ASPNETCORE_URLS="http://+:80"
ENTRYPOINT ["dotnet", "WebApplication1.dll"]

# Optional: Add metadata to the image (useful for tracking/information)
LABEL maintainer="Your Name <your.email@example.com>"
LABEL org.opencontainers.image.source="[https://github.com/your-org/your-repo](https://github.com/your-org/your-repo)"
LABEL org.opencontainers.image.licenses="MIT"
LABEL org.opencontainers.image.description="A simple ASP.NET Core API"
```

**Key Changes:**

* `EXPOSE 80`: The container will now expose port 80 for internal communication.
* `ENV ASPNETCORE_URLS="http://+:80"`: This environment variable ensures your ASP.NET Core application listens on `http://+:80` inside the container, making it accessible to Caddy.

---

## 5. Step 2: Create the `Caddyfile`

The `Caddyfile` is Caddy's configuration. It tells Caddy which domain to serve and where to proxy requests.

**Create a new file named `Caddyfile` (no extension) in your `your-solution-root/` directory:**

```caddyfile
# For local development with HTTP:
# http://localhost

# For local development with HTTPS (Caddy will generate a self-signed cert):
# https://localhost

# For production with a real domain (Caddy will get a Let's Encrypt cert):
your-api-domain.com { # Replace with your actual domain, e.g., api.example.com
    # Optional: Enable automatic HTTP to HTTPS redirection
    # If you use "your-api-domain.com" without "http://" or "https://",
    # Caddy automatically enables HTTP->HTTPS redirection and manages certs.

    # Reverse proxy to your .NET API service
    reverse_proxy webapi:80 { # 'webapi' is the service name in docker-compose, '80' is the internal port
        # Optional: Pass original host header
        header_up Host {http.request.host}
        # Optional: Pass original client IP
        header_up X-Forwarded-For {http.request.remote}
        # Optional: Pass original scheme (http/https)
        header_up X-Forwarded-Proto {http.request.scheme}
    }

    # Optional: Log requests (useful for debugging)
    log {
        output stdout
        format json
    }

    # Optional: Enable Gzip compression (good for APIs returning text/JSON)
    encode gzip zstd

    # Optional: Set security headers
    header {
        # HSTS (HTTP Strict Transport Security) - forces browsers to use HTTPS
        Strict-Transport-Security "max-age=31536000; includeSubDomains; preload"
        # X-Content-Type-Options - prevents MIME sniffing
        X-Content-Type-Options "nosniff"
        # X-Frame-Options - prevents clickjacking
        X-Frame-Options "DENY"
        # X-XSS-Protection - enables XSS filters
        X-XSS-Protection "1; mode=block"
        # Referrer-Policy - controls referrer information sent
        Referrer-Policy "no-referrer-when-downgrade"
        # Content-Security-Policy - very powerful, but requires careful configuration
        # Content-Security-Policy "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline';"
    }
}
```

**Important:**

* **For Production:** Replace `your-api-domain.com` with your actual domain name (e.g., `api.mycompany.com`). Caddy will automatically obtain and manage Let's Encrypt SSL certificates for this domain.
* **For Local Development:** Use `localhost` as the domain. Caddy will generate a self-signed certificate.

---

## 6. Step 3: Create the `docker-compose.yml`

This file defines and orchestrates both your .NET API and Caddy services, sets up a shared network, and manages persistent storage for Caddy's certificates.

**Create a new file named `docker-compose.yml` in your `your-solution-root/` directory:**

```yaml
version: '3.8'

services:
  # Your .NET API Service
  webapi:
    build:
      context: . # Build context is the current directory (solution root)
      dockerfile: WebApplication1/Dockerfile # Path to your API's Dockerfile
      args:
        BUILD_CONFIGURATION: Release # Or Debug for development
    image: your-api-image:latest # Name for your API's Docker image
    container_name: webapi_container
    # No ports exposed here, as Caddy will handle external access
    networks:
      - app_network # Connect to the shared network

  # Caddy Reverse Proxy Service
  caddy:
    image: caddy:2.7.5-alpine # Use a stable Caddy image version
    container_name: caddy_proxy
    restart: unless-stopped # Always restart unless explicitly stopped
    ports:
      - "80:80"   # Expose HTTP for Caddy's automatic redirection to HTTPS
      - "443:443" # Expose HTTPS
    volumes:
      - ./Caddyfile:/etc/caddy/Caddyfile # Mount your local Caddyfile into the container
      - caddy_data:/data               # Volume for Caddy to store certificates and data
    networks:
      - app_network # Connect to the shared network
    depends_on:
      - webapi # Ensure webapi starts before caddy

# Define networks and volumes
networks:
  app_network:
    driver: bridge

volumes:
  caddy_data: # This volume will persist Caddy's certificates and configuration
```

**Explanation of `docker-compose.yml`:**

* **`webapi` service:**
    * `build`: Instructs Docker Compose to build the `webapi` image using the specified `Dockerfile`.
    * `image`: Tags the built image for easy reference.
    * `container_name`: Assigns a readable name to the running container.
    * `networks`: Connects the API service to the `app_network`, allowing Caddy to communicate with it using the service name (`webapi`).
    * **No `ports` mapping**: The API container's port `80` is not exposed directly to the host; only Caddy will communicate with it.
* **`caddy` service:**
    * `image`: Specifies the official Caddy Docker image to use.
    * `ports`: Maps host ports `80` and `443` to the Caddy container's ports, making Caddy accessible from outside.
    * `volumes`:
        * `./Caddyfile:/etc/caddy/Caddyfile`: Mounts your local `Caddyfile` into the Caddy container.
        * `caddy_data:/data`: Creates a Docker volume to persist Caddy's data, including its automatically generated SSL certificates. This is crucial for certificate renewal and persistence across container restarts.
    * `networks`: Connects Caddy to the same `app_network` as the API.
    * `depends_on`: Ensures the `webapi` service starts before Caddy.
* **`networks` and `volumes` sections:** Define the shared network for inter-container communication and the persistent volume for Caddy's data.

---

## 7. How to Run Your Setup

1.  **Save all files** as described in the project structure.
2.  **Open your terminal or command prompt** and navigate to your `your-solution-root/` directory (where `docker-compose.yml` is located).
3.  **Build and run the services** using the following command:

    ```bash
    docker compose up --build -d
    ```
    * `up`: Starts the services defined in `docker-compose.yml`.
    * `--build`: Forces Docker Compose to rebuild the images. This is important after making changes to your `Dockerfile`.
    * `-d`: Runs the containers in detached mode (in the background).

4.  **Verify (Optional):**
    * Check container status:
        ```bash
        docker ps
        ```
    * View Caddy logs (useful for debugging certificate issues or general Caddy behavior):
        ```bash
        docker compose logs caddy
        ```

---

## 8. Accessing Your API

* **If you used `localhost` in your `Caddyfile` (for local development):**
    * Open your web browser and navigate to `https://localhost`.
    * Your browser might display a warning about a self-signed certificate. This is expected and normal for `localhost` HTTPS. You can usually proceed past this warning.

* **If you used a real domain (e.g., `your-api-domain.com`) in your `Caddyfile` (for production):**
    * **Crucial Step:** Ensure your domain's DNS records (A record) point to the public IP address of the server where you are running Docker. Caddy needs this to verify domain ownership and obtain certificates from Let's Encrypt.
    * Open your web browser and navigate to `https://your-api-domain.com`.
    * Caddy will automatically handle the certificate provisioning. You should see a secure connection (padlock icon) in your browser.

---

## 9. Stopping and Removing Services

To stop the running containers and remove the network and volumes created by Docker Compose:

```bash
docker compose down
```

This command will gracefully stop your API and Caddy containers and clean up the associated resources.

---

## 10. Key Concepts and Notes

* **Multi-Stage Builds in Dockerfile**: Your `Dockerfile` uses a multi-stage build (`build`, `publish`, `final`). This is a best practice for .NET applications, resulting in smaller, more secure final images by separating build-time dependencies from runtime dependencies.
* **Docker Layer Caching**: The `COPY` and `RUN dotnet restore` steps are ordered to leverage Docker's build cache. If only your source code changes (not the `.csproj` file), Docker can reuse the cached `dotnet restore` layer, speeding up subsequent builds.
* **`ASPNETCORE_URLS`**: This environment variable is vital for telling your ASP.NET Core application which URLs it should listen on inside the container. `http://+:80` means it listens on port 80 for all incoming connections.
* **Caddy's Automatic HTTPS**: When Caddy is configured with a public domain name, it automatically communicates with Let's Encrypt to obtain and renew SSL certificates. This eliminates manual certificate management.
* **`caddy_data` Volume**: This persistent Docker volume is essential. It stores Caddy's state, including the SSL certificates it obtains. Without it, Caddy would have to re-obtain certificates every time the container restarts, which can lead to rate limiting by Let's Encrypt.
* **Internal Docker Networking**: Docker Compose creates a default network (`app_network` in this case). Services on this network can communicate with each other using their service names (e.g., `webapi:80`). You don't need to know the internal IP address of the `webapi` container.
* **SSL Termination**: In this setup, Caddy performs SSL termination. This means Caddy handles the HTTPS encryption/decryption, and then forwards standard HTTP traffic to your API. Your API doesn't need to be configured for HTTPS internally, simplifying its setup and reducing its attack surface.
* **Security Headers**: The `Caddyfile` includes examples of security headers. These are crucial for enhancing your API's security posture against common web vulnerabilities.
* **Reverse Proxy Benefits**: Beyond HTTPS, Caddy as a reverse proxy can provide other benefits like load balancing (if you scale your API), request logging, and potentially rate limiting.

This document should provide you with a solid foundation for deploying your .NET API with Caddy and Docker Compose.
```
