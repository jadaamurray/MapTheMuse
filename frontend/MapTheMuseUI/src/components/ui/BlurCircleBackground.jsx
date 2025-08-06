import React from 'react';
import { Box } from '@mui/material';
import Ellipse1 from '../../assets/ellipses/Ellipse1.svg';
import Ellipse2 from '../../assets/ellipses/Ellipse2.svg';
import Ellipse3 from '../../assets/ellipses/Ellipse3.svg';
export default function BlurCircleBackground() {
  return (
    <>
      {/* These could be <img> tags pointing at your SVGs in /public too */}
      <Box
        component="img"
        src={Ellipse1}
        alt=""
        sx={{
          position: 'absolute',
          top: '-106px',
          right: '-553px',
          width: '1440px',
          height: '1000px',
          filter: 'blur(75.6px)',
          opacity: 0.8,
          zIndex: 0,
        }}
      />

      <Box
        component="img"
        src={Ellipse2}
        alt=""
        sx={{
          position: 'absolute',
          top: '248px',
          left: '-503px',
          width: '1422px',
          height: '1000px',
          filter: 'blur(50px)',
          zIndex: 0,
        }}
      />

      <Box
        component="img"
        src={Ellipse3}
        alt=""
        sx={{
          position: 'absolute',
          top: '598px',
          left: '-303px',
          width: '1422px',
          height: '1000px',
          filter: 'blur(50px)',
          zIndex: 0,
        }}
      />
    </>
  );
}
