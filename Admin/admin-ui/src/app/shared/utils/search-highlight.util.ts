const escapeRegExp = (value: string): string => value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');

const escapeHtml = (value: string): string =>
  value
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#39;');

export const highlightSearchMatch = (
  value: string | null | undefined,
  searchTerm: string,
  highlightClass = 'search-highlight',
): string => {
  const safeValue = escapeHtml(value ?? '');
  const normalizedSearchTerm = searchTerm.trim();
  if (!normalizedSearchTerm) {
    return safeValue;
  }

  const searchRegex = new RegExp(`(${escapeRegExp(normalizedSearchTerm)})`, 'gi');
  return safeValue.replace(searchRegex, `<mark class="${highlightClass}">$1</mark>`);
};
