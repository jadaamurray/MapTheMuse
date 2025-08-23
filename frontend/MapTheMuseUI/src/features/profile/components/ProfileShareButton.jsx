import ShareIcon from "@mui/icons-material/Share";
import { Button } from "@mui/material";

async function shareOrDownloadImage({ userId, title = "My picks · Map The Muse", text = "" }) {
  const imgUrl = `http://localhost:5062/og/collage/user/${userId}.png`;

  // Fetch the PNG
  const res = await fetch(imgUrl, { mode: "cors", cache: "no-cache" });
  if (!res.ok) throw new Error("Failed to fetch collage image");
  const blob = await res.blob();

  // Wrap as File
  const file = new File([blob], "mapthemuse-collage.png", { type: "image/png" });

  // Web Share with files
  if (navigator.canShare?.({ files: [file] })) {
    try {
      await navigator.share({ files: [file], title, text });
      return;
    } catch (e) {
      // user cancelled or app rejected; fall through to download
    }
  }

  // Fallback: download so the user can upload manually
  const url = URL.createObjectURL(blob);
  const a = document.createElement("a");
  a.href = url;
  a.download = "mapthemuse-collage.png";
  document.body.appendChild(a);
  a.click();
  a.remove();
  URL.revokeObjectURL(url);
}

export default function ShareImageButton({ user }) {
  const onClick = () =>
    shareOrDownloadImage({
      userId: user.id,
      title: "My picks · Map The Muse",
      text: "Destinations & media I love",
    }).catch(err => {
      console.error(err);
      alert("Couldn’t share automatically. The image was downloaded instead.");
    });

  return (
    <Button variant="outlined" startIcon={<ShareIcon />} sx={{ borderRadius: 3 }} onClick={onClick}>
      Share travel style
    </Button>
  );
}