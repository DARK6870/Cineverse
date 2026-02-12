export function decodeTokenPayload(token: string): any {
  const payloadBase64 = token.split('.')[1];
  const payloadJson = decodeURIComponent(
    atob(payloadBase64)
      .split('')
      .map(c => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
      .join('')
  );
  return JSON.parse(payloadJson);
}
