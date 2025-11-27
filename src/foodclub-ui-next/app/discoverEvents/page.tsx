"use client";

import { useEffect, useRef, useState } from "react";
import { Button } from "@/components/ui/button";
import MealCard from "@/components/core/meal-card/meal-card";
import { searchEventsBySelectedFilters } from "@/services/api/events-api.service";
import type { EventOverviewDto } from "@/types/event-details.type";
import type { SearchEventsRequestDto } from "@/types/search-events.type";

export default function DiscoverEvents() {
  const [searchTerm, setSearchTerm] = useState("");
  const [price, setPrice] = useState<number>(500);
  const [fromDate, setFromDate] = useState<string>("");
  const [toDate, setToDate] = useState<string>("");
  const [minAge, setMinAge] = useState<number | null>(null);
  const [maxAge, setMaxAge] = useState<number | null>(null);
  const [isPublic, setIsPublic] = useState<boolean>(true);

  const [events, setEvents] = useState<EventOverviewDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const controllerRef = useRef<AbortController | null>(null);

  useEffect(() => {
    // cleanup on unmount
    return () => {
      controllerRef.current?.abort();
    };
  }, []);

  const handleSearch = async () => {
    setError(null);
    setLoading(true);
    controllerRef.current?.abort();
    const controller = new AbortController();
    controllerRef.current = controller;

    const dto: Partial<SearchEventsRequestDto> = {
      locationOrEventName: searchTerm || undefined,
      minPrice: 0,
      maxPrice: price,
      fromDate: fromDate || undefined,
      toDate: toDate || undefined,
      minAge,
      maxAge,
      isPublic,
    };

    try {
      window.dispatchEvent(new CustomEvent("app:fetch-start"));
      const result = await searchEventsBySelectedFilters(
        dto as Partial<SearchEventsRequestDto>,
        controller.signal
      );
      setEvents(result || []);
    } catch (err) {
      if ((err as any)?.name === "AbortError") return;
      console.error(err);
      setError("Failed to fetch events");
    } finally {
      window.dispatchEvent(new CustomEvent("app:fetch-end"));
      setLoading(false);
    }
  };

  useEffect(() => {
    handleSearch();
  }, []);

  return (
    <main className="max-w-7xl mx-auto px-6 py-8">
      <header className="mb-8">
        <h1 className="text-3xl font-bold">Search for events</h1>
      </header>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-8">
        <aside className="lg:col-span-3">
          <div className="bg-white rounded-lg p-6 shadow-sm">
            <h2 className="text-base font-semibold mb-4">Filters</h2>

            <label className="block text-xs text-muted-gray font-bold mb-2">
              Search
            </label>
            <input
              type="search"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Search city or meal..."
              className="flex-1 h-10 px-3 rounded-md border bg-gray-50 mb-4"
            />

            <label className="block text-xs text-muted-gray font-bold mb-2 gap-6">
              Price (DKK)
            </label>
            <div className="flex items-center gap-3 mb-2">
              <input
                type="range"
                min="0"
                max="500"
                value={price}
                onChange={(e) => setPrice(Number(e.target.value))}
                className="w-full"
              />
              <div className="text-sm font-semibold">DKK {price}</div>
            </div>
            <br />
            <label className="block text-xs text-muted-gray font-bold mb-2">
              Date
            </label>
            <div className="mb-4">
              <div className="mt-2">
                <div className="flex flex-col sm:flex-row items-center gap-2">
                  <label htmlFor="date-from">From</label>
                  <input
                    id="date-from"
                    type="date"
                    value={fromDate}
                    onChange={(e) => setFromDate(e.target.value)}
                    className="h-10 px-3 rounded-md border bg-white w-full sm:w-auto"
                  />
                </div>
                <br />
                <div className="flex flex-col sm:flex-row items-center gap-2">
                  <label htmlFor="date-to">To &nbsp;&nbsp;&nbsp;&nbsp;</label>
                  <input
                    id="date-to"
                    type="date"
                    value={toDate}
                    onChange={(e) => setToDate(e.target.value)}
                    className="h-10 px-3 rounded-md border bg-white w-full sm:w-auto"
                  />
                </div>
              </div>
            </div>
            <br />
            <label className="block text-xs text-muted-gray font-bold mb-2">
              Age Range
            </label>
            <div className="flex items-center gap-2 mb-4">
              <label htmlFor="age-from">From</label>
              <input
                id="age-from"
                type="number"
                min={18}
                max={98}
                value={minAge ?? undefined}
                onChange={(e) => setMinAge(Number(e.target.value))}
                className="h-10 px-3 rounded-md border w-20"
                aria-label="Minimum age"
              />
              <label htmlFor="age-to">To</label>
              <input
                id="age-to"
                type="number"
                min={19}
                max={99}
                value={maxAge ?? undefined}
                onChange={(e) => setMaxAge(Number(e.target.value))}
                className="h-10 px-3 rounded-md border w-20"
                aria-label="Maximum age"
              />
            </div>

            <div className="flex items-center gap-3 mb-4">
              <label className="text-sm">Is Public</label>
              <input
                type="checkbox"
                checked={isPublic}
                onChange={(e) => setIsPublic(e.target.checked)}
                aria-label="Is Public"
              />
            </div>
            <br />
            <div className="flex justify-end">
              <Button
                variant="default"
                className="bg-emerald-800 hover:bg-emerald-700"
                onClick={handleSearch}
              >
                {loading ? "Searching…" : "Search"}
              </Button>
            </div>
          </div>
        </aside>
        <section className="lg:col-span-9">
          <div className="flex items-center justify-between mb-6">
            <div className="text-sm text-gray-600">
              Showing results based on selected search parameters
            </div>
            <div className="flex items-center gap-3 text-sm">
              <label className="hidden sm:inline">Sort</label>
              <select className="h-8 px-2 border rounded-md bg-white">
                <option>Recommended</option>
                <option>Price: Low to High</option>
                <option>Price: High to Low</option>
              </select>
            </div>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {loading &&
              Array.from({ length: 6 }).map((_, i) => (
                <div
                  key={i}
                  className="h-56 bg-gray-50 rounded-lg border border-gray-100 shadow-sm"
                />
              ))}

            {!loading && events.length === 0 && (
              <div className="col-span-full text-center text-gray-500 py-12">
                No events found
              </div>
            )}

            {!loading &&
              events.map((eventDetail) => (
                <MealCard key={eventDetail.eventId} {...eventDetail} />
              ))}
          </div>
        </section>
      </div>
    </main>
  );
}
