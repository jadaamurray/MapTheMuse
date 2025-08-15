import apiClient from "../../../api/apiClient";

export async function searchDestinations({ continent, factKey } = {}) {
  const params = {};
  if (continent) params.continent = continent;
  if (factKey) params.factKey = factKey;
  const { data } = await apiClient.get("/destinations/?", { params });
  return data;
}
