import React from 'react';
import PropTypes from 'prop-types';
import { Button } from '@mui/material';

export default function HeaderButton({
  label,
  active = false,
  disabled = false,
  onClick,
}) {
  return (
    <Button
      onClick={onClick}
      disabled={disabled}
      variant="text"
      sx={{
        textTransform: 'none',
        fontSize: '16px',
        fontWeight: 400,
        color: disabled
          ? 'text.disabled'
          : active
          ? 'primary.main'
          : 'text.primary',
        borderRadius: 0,
        borderBottom: '2px solid transparent',
        borderColor:  'transparent',
        px: 1.5,   // horizontal padding
        py: 0.5,   // vertical padding
        // hover underline
        '&:hover': {
          backgroundColor: disabled ? 'inherit' : 'action.hover',
          borderBottomColor: active ? 'transparent': 'primary.dark',    // change border color on hover if active
        },
        //  Click (mousedown) effect
        '&:active': {
            borderBottomColor: 'primary.main',
            borderBottom  : '2px solid',
            fontWeight: 600,
          boxShadow: '0px 0px 0px 2px rgba(16, 55, 74, 0.2)', // subtle glow effect
        },
      }}
    >
      {label}
    </Button>
  );
}

HeaderButton.propTypes = {
  label: PropTypes.string.isRequired,
  active: PropTypes.bool,
  disabled: PropTypes.bool,
  onClick: PropTypes.func,
};
