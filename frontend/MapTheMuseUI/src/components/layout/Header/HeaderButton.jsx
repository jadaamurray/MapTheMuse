import React from 'react';
import PropTypes from 'prop-types';
import { Button } from '@mui/material';

export default function HeaderButton({
  label,
  active,
  disabled,
  onClick,
  color = "primary",

}) {
  return (
    <Button
      onClick={onClick}
      disabled={disabled}
      variant="text"
      sx={{
        textTransform: "none",
        fontFamily: 'Inter',
        fontSize: "16px",
        fontWeight: 500,
        color: disabled
          ? "text.disabled"
          : active
          ? `${color}.main`
          : "text.primary",
        borderRadius: 0,
        borderBottom: "2px solid transparent",
        borderColor: "transparent",
        px: 1.5,
        py: 0.5,
        "&:hover": {
          backgroundColor: disabled ? "inherit" : "action.hover",
          borderBottomColor: active
            ? "transparent"
            : `${color}.dark`,
          color: disabled ? "text.disabled" : `${color}.main`, 
        },
        "&:active": {
          borderBottomColor: `${color}.main`,
          borderBottom: "2px solid",
          fontWeight: 600,
          boxShadow: `0px 0px 0px 2px rgba(16, 55, 74, 0.2)`,
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
