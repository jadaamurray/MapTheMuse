import { Box, Stack, Typography, Card } from "@mui/material";
import PropTypes from "prop-types";
import React from "react";

export default function FeatureCard({
  subtitle = "",
  title = "",
  description = "",
  Icon,
  iconSize = 120,
  // allow string or fn(theme) for bg
  bg = (t) => t.palette.background.cream,
  fg = (t) => t.palette.primary.main,
  sx = {},
  titleProps = {},
  subtitleProps = {},
  descriptionProps = {},
}) {
  return (
    <Card
    variant="outlined"
      sx={(theme) => ({
        width: 289,
        height: 379,
        background: bg,
        //borderRadius: 2,
        p: "12px 21px",
        overflow: "hidden",
        /*boxShadow: `
          10px 16px 9.8px 3px rgba(12, 12, 13, 0.10),
          inset 2px 11px 20.6px rgba(255, 255, 255, 0.25)
        `, */
        ...(typeof sx === "function" ? sx(theme) : sx),
      })}
    >
      <Stack spacing={5} alignItems="center" height="100%">
        <Stack spacing={1} alignItems="flex-start" width="100%">
          <Typography
            sx={{
              fontFamily: "inter",
              fontWeight: 550,
              color: "text.primary",
              fontSize: 20,
              lineHeight: 1.2,
            }}
            {...subtitleProps}
          >
            {subtitle}
          </Typography>

          <Typography
            sx={{
              fontFamily: "Outfit",
              fontWeight: 800,
              color: fg,
              fontSize: 40,
              lineHeight: 1.1,
            }}
            {...titleProps}
          >
            {title}
          </Typography>
        </Stack>

        {Icon ? <Icon sx={{ width: iconSize, height: iconSize, color: fg }} /> : null}

        <Typography
          sx={{
            fontFamily: "Inter",
            fontWeight: 500,
            color: "text.primary",
            fontSize: 16,
            lineHeight: "22.4px",
            width: 258,
            textAlign: "left",
          }}
          {...descriptionProps}
        >
          {description}
        </Typography>
      </Stack>
    </Card>
  );
}

FeatureCard.propTypes = {
  title: PropTypes.string,
  subtitle: PropTypes.string,
  description: PropTypes.string,
  Icon: PropTypes.elementType,
  iconSize: PropTypes.number,
  bg: PropTypes.oneOfType([PropTypes.string, PropTypes.func]),
  fg: PropTypes.string,
  sx: PropTypes.oneOfType([PropTypes.object, PropTypes.func]),
  titleProps: PropTypes.object,
  subtitleProps: PropTypes.object,
  descriptionProps: PropTypes.object,
};
