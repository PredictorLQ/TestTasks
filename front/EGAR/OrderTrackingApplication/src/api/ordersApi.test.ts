import { describe, expect, it, vi } from 'vitest';
import { createOrderStatusEventSource, ordersApi } from './ordersApi';
import { createMockOrder } from '../test/testUtils';

describe('ordersApi', () => {
  it('getAll fetches orders', async () => {
    const orders = [createMockOrder()];
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: true,
        json: async () => orders,
      }),
    );

    await expect(ordersApi.getAll()).resolves.toEqual(orders);
    expect(fetch).toHaveBeenCalledWith('/api/orders');
  });

  it('create sends POST request', async () => {
    const order = createMockOrder();
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: true,
        json: async () => order,
      }),
    );

    await expect(
      ordersApi.create({ orderNumber: 'ORD-001', description: 'Test' }),
    ).resolves.toEqual(order);

    expect(fetch).toHaveBeenCalledWith('/api/orders', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ orderNumber: 'ORD-001', description: 'Test' }),
    });
  });

  it('throws on failed response', async () => {
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: false,
        status: 400,
        text: async () => 'Bad request',
      }),
    );

    await expect(ordersApi.getAll()).rejects.toThrow('Bad request');
  });

  it('createOrderStatusEventSource builds url with orderId', () => {
    const close = vi.fn();
    class MockEventSource {
      url: string;
      close = close;

      constructor(url: string) {
        this.url = url;
      }
    }

    vi.stubGlobal('EventSource', MockEventSource);

    const source = createOrderStatusEventSource('abc-123');

    expect(source.url).toBe('/api/ordernotifications/stream?orderId=abc-123');
  });
});
