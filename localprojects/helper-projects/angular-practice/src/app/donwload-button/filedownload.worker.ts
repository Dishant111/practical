/// <reference lib="webworker" />

addEventListener('message', ({ data }) => {
  const { encryptedBuffer, ivHeader, originalFileName } = data;
  const response = `worker response to ${data}`;
  console.log('Worker received : data');
  // decrypte here and send decrypted butffer back
  postMessage(
    {
      status: 'done',
      decryptedBuffer: encryptedBuffer,
      fileName: originalFileName,
    },
    [encryptedBuffer]
  ); // Use Transferable Objects for performance
});
