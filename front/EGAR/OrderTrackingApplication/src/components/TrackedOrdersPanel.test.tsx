import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { beforeEach, describe, expect, it } from 'vitest';
import { createMockOrder } from '../test/testUtils';
import { useOrderStore } from '../store/orderStore';
import { TrackedOrdersPanel } from './TrackedOrdersPanel';

describe('TrackedOrdersPanel', () => {
  beforeEach(() => {
    useOrderStore.setState({ orders: [], loading: false, error: null, trackedOrderIds: [] });
  });

  it('shows empty message when no tracked orders', () => {
    render(
      <MemoryRouter>
        <TrackedOrdersPanel />
      </MemoryRouter>,
    );

    expect(screen.getByText(/Добавьте заказы в отслеживание/)).toBeInTheDocument();
  });

  it('lists tracked orders', () => {
    const order = createMockOrder({ orderNumber: 'ORD-TRACKED' });
    useOrderStore.setState({
      orders: [order],
      trackedOrderIds: [order.id],
    });

    render(
      <MemoryRouter>
        <TrackedOrdersPanel />
      </MemoryRouter>,
    );

    expect(screen.getByText('ORD-TRACKED')).toBeInTheDocument();
  });
});
