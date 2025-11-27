import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";
import type { SearchEventsRequestDto } from "@/types/search-events.type";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function toQueryParams(dto: Partial<SearchEventsRequestDto>): string {
  const params = new URLSearchParams();

  if (
    dto.locationOrEventName &&
    String(dto.locationOrEventName).trim() !== ""
  ) {
    params.set("locationOrEventName", String(dto.locationOrEventName));
  }

  if (typeof dto.minPrice === "number" && !Number.isNaN(dto.minPrice)) {
    params.set("minPrice", String(dto.minPrice));
  }

  if (
    typeof dto.maxPrice === "number" &&
    !Number.isNaN(dto.maxPrice) &&
    dto.maxPrice > 0
  ) {
    params.set("maxPrice", String(dto.maxPrice));
  }

  if (dto.fromDate && String(dto.fromDate).trim() !== "") {
    params.set("fromDate", String(dto.fromDate));
  }

  if (dto.toDate && String(dto.toDate).trim() !== "") {
    params.set("toDate", String(dto.toDate));
  }

  if (
    typeof dto.minAge === "number" &&
    !Number.isNaN(dto.minAge) &&
    dto.minAge >= 18
  ) {
    params.set("minAge", String(dto.minAge));
  }

  if (
    typeof dto.maxAge === "number" &&
    !Number.isNaN(dto.maxAge) &&
    dto.maxAge > 18
  ) {
    params.set("maxAge", String(dto.maxAge));
  }

  if (typeof dto.isPublic === "boolean") {
    params.set("isPublic", String(dto.isPublic));
  }

  const s = params.toString();
  return s ? `?${s}` : "";
}
