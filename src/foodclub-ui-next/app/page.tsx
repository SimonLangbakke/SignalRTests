"use client";
import Link from "next/link";
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
} from "@/components/ui/card";
import { useState, useEffect } from "react";
import MealCard from "@/components/core/meal-card/meal-card";
import { getRecentEventsForHomePage } from "@/services/api/events-api.service";
import { EventOverviewDto } from "@/types/event-details.type";

export default function Home() {
  const [eventDetails, setEvents] = useState<Array<EventOverviewDto>>([]);
  useEffect(() => {
    let isSubscribed = true;
    const controller = new AbortController();

    const fetchEvents = async () => {
      try {
        const events = await getRecentEventsForHomePage();
        if (isSubscribed) {
          setEvents(events);
        }
      } catch (err) {
        console.error("Failed to fetch event details:", err);
      }
    };

    fetchEvents();

    return () => {
      isSubscribed = false;
      controller.abort();
    };
  }, []);
  return (
    <div className="bg-white">
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Hero */}
        <section className="bg-amber-50 rounded-lg mt-6 p-6 md:p-10 flex flex-col-reverse md:flex-row items-center gap-6">
          <div className="md:w-2/3">
            <h1 className="text-3xl sm:text-4xl md:text-5xl font-extrabold text-gray-900 leading-tight">
              Share meals. Reduce waste. Meet neighbors.
            </h1>
            <p className="mt-3 text-base sm:text-lg text-gray-600">
              Book or host home-cooked dinners across Denmark.
            </p>

            <div className="mt-6 flex flex-wrap gap-3">
              <Link
                href="/discoverEvents"
                className="inline-flex items-center justify-center px-6 py-2 rounded-full bg-emerald-800 text-white font-semibold hover:bg-emerald-700"
              >
                Discover events
              </Link>
              <Link
                href="#"
                className="inline-flex items-center justify-center px-6 py-2 rounded-full border border-emerald-800 text-emerald-800 font-semibold bg-white hover:bg-emerald-50"
              >
                Host a meal
              </Link>
            </div>
          </div>

          <div className="md:w-1/3 flex justify-center">
            <img
              src="/home_logo.png"
              alt="Fresh produce"
              className="w-48 sm:w-56 md:w-64 object-contain"
            />
          </div>
        </section>

        {/* Why section */}
        <section className="mt-12">
          <h2 className="text-2xl font-bold text-gray-900">Why Plateful?</h2>
          <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            <Card className="border-gray-200 shadow-sm">
              <CardHeader>
                <CardTitle className="text-lg font-bold">
                  Affordable Meals
                </CardTitle>
                <CardDescription className="text-sm text-gray-600">
                  Enjoy home-cooked meals at fair prices while sharing costs.
                </CardDescription>
              </CardHeader>
            </Card>

            <Card className="border-gray-200 shadow-sm">
              <CardHeader>
                <CardTitle className="text-lg font-bold">
                  Meet new people
                </CardTitle>
                <CardDescription className="text-sm text-gray-600">
                  Build local connections over shared meals.
                </CardDescription>
              </CardHeader>
            </Card>

            <Card className="border-gray-200 shadow-sm">
              <CardHeader>
                <CardTitle className="text-lg font-bold">
                  Reduce food waste
                </CardTitle>
                <CardDescription className="text-sm text-gray-600">
                  Cook big, split portions, and save what matters.
                </CardDescription>
              </CardHeader>
            </Card>
          </div>
        </section>

        {/* Upcoming dinners */}
        <section className="mt-12 mb-16">
          <h2 className="text-2xl font-bold text-gray-900">Upcoming dinners</h2>
          <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {eventDetails.length === 0
              ? // show placeholders when no events
                Array.from({ length: 6 }).map((_, i) => (
                  <div
                    key={i}
                    className="h-64 bg-gray-50 rounded-lg border border-gray-100 shadow-sm"
                  />
                ))
              : eventDetails.map((eventDetail) => (
                  <MealCard key={eventDetail.eventId} {...eventDetail} />
                ))}
          </div>
        </section>
      </main>
    </div>
  );
}
