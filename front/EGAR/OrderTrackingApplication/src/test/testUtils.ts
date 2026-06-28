import type { Order } from '../types/order';

export function createMockOrder(overrides: Partial<Order> = {}): Order {
  return {
    id: '11111111-1111-1111-1111-111111111111',
    orderNumber: 'ORD-001',
    description: 'Test order',
    status: 'Created',
    createdAt: '2026-06-28T10:00:00.000Z',
    updatedAt: '2026-06-28T10:00:00.000Z',
    ...overrides,
  };
}
