export function buildProfilePatch(oldVals, newVals) {
  const keys = ["firstName","lastName","country","preferredLanguage","profilePictureUrl"];
  const trim = (v) => (typeof v === "string" ? v.trim() : v);
  const ops = [];

  keys.forEach((k) => {
    const prevVal = trim(oldVals?.[k] ?? "");
    const nextVal = trim(newVals?.[k] ?? "");
    if (prevVal !== nextVal) {
      ops.push({ op: "replace", path: `/${k}`, value: nextVal });
    }
  });

  return ops;
}