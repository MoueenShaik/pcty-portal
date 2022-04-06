import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';

test('renders Employee Benefits Cost button', () => {
  render(<App />);
  const linkElement = screen.getByText(/Get Employee Benefits Cost/i);
  expect(linkElement).toBeInTheDocument();
});
