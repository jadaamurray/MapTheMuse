import { Box, List, ListItemButton, Stack, Typography } from '@mui/material';

export default function LinkList({ title, items }) {
  return (
    <Box>
      <Typography variant="h6" color="text.primary" gutterBottom>
        {title}
      </Typography>
      <List disablePadding>
        {items.map((item) => (
          <ListItemButton
            key={item}
            disableGutters
            sx={{ mb: 1 }} // 8px bottom margin
            onClick={() => console.log(`Clicked ${item}`)}
          >
            <Typography variant="body1" color="text.primary">
              {item}
            </Typography>
          </ListItemButton>
        ))}
      </List>
    </Box>
  );
}