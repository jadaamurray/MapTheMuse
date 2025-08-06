import { Box, List, ListItemButton, Stack, Typography } from '@mui/material';
import LinkList from '../ui/LinkList';

export default function Footer() {
  const features = ['Destinations', 'Music', 'Books', 'TV Shows', 'Movies', 'Painting & Sculpture', 'Architecture'];
  const articles = ['Travel Tips', 'Destination Guides', 'User Stories', 'Interviews', 'How-To Articles'];
  const resources = ['API Docs', 'Developer Guides', 'FAQs', 'Support', 'Community Forum'];
  const company = ['About Us', 'Careers', 'Press', 'Terms of Service', 'Privacy Policy'];

  return (
    <Box
      component="footer"
      sx={{
        bgcolor: 'background.paper',
        py: 4,
        px: 2,
        borderTop: '1px solid',
        borderColor: 'divider',
      }}
    >
      <Stack
        direction={{ xs: 'column', sm: 'row' }}
        spacing={4}
        justifyContent="space-between"
        maxWidth="lg"
        mx="auto"
      >
        <LinkList title="Features" items={features} />
        <LinkList title="Articles" items={articles} />
        <LinkList title="Resources" items={resources} />
        <LinkList title="Company" items={company} />
      </Stack>
    </Box>
  );
}
