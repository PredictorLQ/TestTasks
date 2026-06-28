import { describe, expect, it } from 'vitest';
import { formatDateTime, getStatusLabel, getStatusVariant } from '../utils/statusLabels';

describe('statusLabels', () => {
  it('returns Russian labels for all statuses', () => {
    expect(getStatusLabel('Created')).toBe('Создан');
    expect(getStatusLabel('Sent')).toBe('Отправлен');
    expect(getStatusLabel('Delivered')).toBe('Доставлен');
    expect(getStatusLabel('Cancelled')).toBe('Отменён');
  });

  it('returns bootstrap variants for statuses', () => {
    expect(getStatusVariant('Created')).toBe('secondary');
    expect(getStatusVariant('Sent')).toBe('primary');
    expect(getStatusVariant('Delivered')).toBe('success');
    expect(getStatusVariant('Cancelled')).toBe('danger');
  });

  it('formats datetime in ru-RU locale', () => {
    const formatted = formatDateTime('2026-06-28T12:00:00.000Z');
    expect(formatted).toMatch(/2026/);
    expect(formatted).toMatch(/28/);
  });
});
