import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { BaseBtn } from '../base-btn';

@Component({
  selector: 'app-donwload-button',
  imports: [BaseBtn],
  templateUrl: './donwload-button.html',
  styleUrl: './donwload-button.scss',
})
export class DonwloadButton {
  constructor(private http: HttpClient) {}

  donwlaodFile() {
    // 1.
    // window.open('https://localhost:7119/File/token-download?token=1a2b3c4d&fileId=123', '_blank');

    // 2.
    this.http
      .get('https://localhost:7119/File/token-download?token=1a2b3c4d&fileId=123', {
        responseType: 'arraybuffer',
        observe: 'response',
      })
      .subscribe({
        next: (response) => {
          const encryptedBuffer = response.body as ArrayBuffer;
          const originalFileName =
            response.headers.get('Content-Disposition')?.match(/filename="?(.+)"?/)?.[1] ||
            'large.mkv';

          // 2. Instantiate the Web Worker
          const worker = new Worker(new URL('./filedownload.worker', import.meta.url));

          // 3. Define the Worker's response handler
          worker.onmessage = (event) => {
            const data = event.data;
            if (data.status === 'done') {
              // 4. Create Blob and trigger download on the main thread
              const decryptedBlob = new Blob([data.decryptedBuffer], {
                type: 'application/octet-stream',
              });
              this.triggerDownload(decryptedBlob, data.fileName);
              worker.terminate(); // Kill the worker when done
            } else if (data.status === 'error') {
              console.error('Worker decryption error:', data.error);
              worker.terminate();
            }
          };

          // 5. Send the encrypted data to the worker to start decryption
          // We pass the ArrayBuffer as a Transferable Object ([encryptedBuffer])
          // for zero-copy, highly efficient data transfer.
          worker.postMessage(
            {
              encryptedBuffer: encryptedBuffer,
              ivHeader: 'header-example', // Replace with actual IV header if needed
              originalFileName: originalFileName,
            },
            [encryptedBuffer]
          );
        },
        error: (err) => {
          console.error('Download failed:', err);
        },
      });
  }

  private triggerDownload(blob: Blob, fileName: string): void {
    const dataUrl = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = dataUrl;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    window.URL.revokeObjectURL(dataUrl);
    document.body.removeChild(link);
  }
}
