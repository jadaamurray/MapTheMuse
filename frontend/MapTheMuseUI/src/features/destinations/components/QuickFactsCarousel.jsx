import React, { useMemo, useRef } from "react";
import { Box, Card, CardContent, IconButton, Typography, Divider } from "@mui/material";
import ArrowBackIosNew from "@mui/icons-material/ArrowBackIosNew";
import ArrowForwardIos from "@mui/icons-material/ArrowForwardIos";
import CardRail from "../../../components/ui/CardRail";

/**
 * Props:
 *  - facts: object like { "Capital": "Bangkok", "Currency": "THB", ... }
 *  - title?: string
 *  - cardWidth?: number (px, default ~280)
 */
export default function QuickFactsCarousel({ facts, title = "Quick facts", cardWidth = 280 }) {
  const items = useMemo(() => Object.entries(facts || {}), [facts]);
  const ref = useRef(null);

  const scrollBy = (dir) => {
    const el = ref.current;
    if (!el) return;
    // scroll roughly one full card (including gap)
    const gap = 16; // must match container gap
    el.scrollBy({ left: dir * (cardWidth + gap), behavior: "smooth" });
  };

  if (!items.length) return null;

  return (
    <Box sx={{ position: "relative", width: "100%" }}>
      {/* Header */}
      <Box sx={{ display: "flex", alignItems: "center", mb: 2 }}>
        <Typography variant="h2" sx={{ flex: 1 }}>{title}</Typography>
      </Box>

      {/* Rail */}
      <CardRail
      >
        {items.map(([label, value], idx) => (
          <Card
            key={`${label}-${idx}`}
            sx={{
              flex: "0 0 auto",
              width: cardWidth,
              scrollSnapAlign: "start",
              borderRadius: 2,
              boxShadow: "0 2px 10px rgba(0,0,0,.12)",
            }}
          >
            <CardContent sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
              <Typography variant="overline" sx={{ opacity: 0.7 }}>
                {label}
              </Typography>
              <Divider />
              <Typography variant="body1">
                {value}
              </Typography>
            </CardContent>
          </Card>
        ))}
      </CardRail>
    </Box>
  );
}
