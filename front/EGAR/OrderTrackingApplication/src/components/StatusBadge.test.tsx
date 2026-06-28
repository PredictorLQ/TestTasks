import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';
import { StatusBadge } from './StatusBadge';

describe('StatusBadge', () => {
  it('renders localized status label', () => {
    render(<StatusBadge status="Sent" />);

    expect(screen.getByText('Отправлен')).toBeInTheDocument();
  });

  it('renders cancelled status', () => {
    render(<StatusBadge status="Cancelled" />);

    expect(screen.getByText('Отменён')).toBeInTheDocument();
  });
});
