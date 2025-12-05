export function formatDate(stringDate: string): string {
  if (!stringDate) return '';
  const date = new Date(stringDate);
  return date.toLocaleDateString('en-US', {
    day: 'numeric',
    month: 'long',
  });
}
