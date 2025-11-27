import { StdioNull } from "node:child_process";

export type SearchEventsRequestDto = {
  locationOrEventName: string | null;
  minPrice: number | StdioNull;
  maxPrice: number | null;
  fromDate: string | null;
  toDate: string | null;
  minAge: number | null;
  maxAge: number | null;
  isPublic: boolean | null;
};
