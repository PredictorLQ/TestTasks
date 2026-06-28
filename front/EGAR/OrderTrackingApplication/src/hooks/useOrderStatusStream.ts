import { useEffect } from 'react';
import { createOrderStatusEventSource } from '../api/ordersApi';
import type { OrderStatusNotification } from '../types/order';

export function useOrderStatusStream(
  onNotification: (notification: OrderStatusNotification) => void,
  orderId?: string,
  enabled = true,
) {
  useEffect(() => {
    if (!enabled) {
      return;
    }

    const source = createOrderStatusEventSource(orderId);

    source.onmessage = (event) => {
      try {
        const notification = JSON.parse(event.data) as OrderStatusNotification;
        onNotification(notification);
      } catch {
        // ignore malformed events
      }
    };

    source.onerror = () => {
      source.close();
    };

    return () => {
      source.close();
    };
  }, [orderId, enabled, onNotification]);
}
