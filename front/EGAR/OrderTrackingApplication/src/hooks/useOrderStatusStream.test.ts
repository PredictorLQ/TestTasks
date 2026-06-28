import { renderHook, waitFor } from '@testing-library/react';
import { afterEach, describe, expect, it, vi } from 'vitest';
import { useOrderStatusStream } from './useOrderStatusStream';

type MessageHandler = ((event: MessageEvent) => void) | null;

class MockEventSource {
  static instances: MockEventSource[] = [];
  url: string;
  onmessage: MessageHandler = null;
  onerror: (() => void) | null = null;
  close = vi.fn();

  constructor(url: string) {
    this.url = url;
    MockEventSource.instances.push(this);
  }

  emit(data: unknown) {
    this.onmessage?.({ data: JSON.stringify(data) } as MessageEvent);
  }
}

describe('useOrderStatusStream', () => {
  afterEach(() => {
    MockEventSource.instances = [];
    vi.unstubAllGlobals();
  });

  it('subscribes to SSE and handles notifications', async () => {
    vi.stubGlobal('EventSource', MockEventSource);
    const onNotification = vi.fn();

    renderHook(() =>
      useOrderStatusStream(onNotification, 'order-123', true),
    );

    await waitFor(() => {
      expect(MockEventSource.instances).toHaveLength(1);
    });

    expect(MockEventSource.instances[0].url).toBe(
      '/api/ordernotifications/stream?orderId=order-123',
    );

    MockEventSource.instances[0].emit({
      orderId: 'order-123',
      orderNumber: 'ORD-001',
      previousStatus: 'Created',
      newStatus: 'Sent',
      occurredAt: '2026-06-28T10:00:00.000Z',
    });

    expect(onNotification).toHaveBeenCalledWith({
      orderId: 'order-123',
      orderNumber: 'ORD-001',
      previousStatus: 'Created',
      newStatus: 'Sent',
      occurredAt: '2026-06-28T10:00:00.000Z',
    });
  });

  it('does not subscribe when disabled', () => {
    vi.stubGlobal('EventSource', MockEventSource);
    const onNotification = vi.fn();

    renderHook(() => useOrderStatusStream(onNotification, 'order-123', false));

    expect(MockEventSource.instances).toHaveLength(0);
  });
});
