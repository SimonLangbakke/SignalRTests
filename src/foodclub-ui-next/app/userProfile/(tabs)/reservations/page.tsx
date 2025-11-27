"use client";

import { Button } from "@/components/ui/button";
import { parseJwt } from "@/lib/jwt-decoder.helper";
import { getEventsByUserAsParticipant } from "@/services/api/events-api.service";
import { EventOverviewDto } from "@/types/event-details.type";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Reservations() {
  const [userEvents, setUserEvents] = useState<Array<EventOverviewDto>>([]);
  const router = useRouter();

  useEffect(() => {
    let isSubscribed = true;
    const controller = new AbortController();

    const fetchEvents = async () => {
      const token = localStorage.getItem("accessToken");
      if (!token) {
        router.replace("/");
        return;
      }

      try {
        const usertokenProps = parseJwt(token);
        const events = await getEventsByUserAsParticipant(
          usertokenProps.sub,
          token
        );
        if (isSubscribed) {
          setUserEvents(events);
        }
      } catch (err) {
        console.error("Failed to fetch event details:", err);
        if (err instanceof Error && err.message.includes("Invalid token")) {
          router.replace("/");
        }
      }
    };

    fetchEvents();

    return () => {
      isSubscribed = false;
      controller.abort();
    };
  }, []);
  return (
    <div>
      <h2 className="text-xl font-bold mb-6">My Reservations</h2>

      <div className="space-y-4">
        {userEvents.length === 0 ? (
          <p className="text-gray-500 text-center py-8">
            No event reservations found
          </p>
        ) : (
          userEvents.map((event) => (
            <div
              key={event.eventId}
              className="bg-white rounded-lg p-4 flex items-center gap-4"
            >
              {event.imageThumbnail ? (
                <img
                  src={event.imageThumbnail}
                  alt={event.name}
                  className="w-20 h-20 object-cover rounded-lg"
                />
              ) : (
                <div className="w-20 h-20 bg-gray-100 rounded-lg"></div>
              )}

              <div className="flex-1">
                <h3 className="font-semibold mb-1">{event.name}</h3>
                <p className="text-gray-600 text-sm">
                  Hosted by • {event.hostName} •{" "}
                  {`${event.startDate} ${event.startTime}`} • {event.price} DKK
                </p>
                <p className="text-emerald-600 text-sm mt-1">
                  Confirmed • {event.participantsCount} guests
                </p>
              </div>

              <Button
                variant="outline"
                className="border-emerald-800 text-emerald-800 hover:bg-emerald-50"
              >
                View Details
              </Button>
            </div>
          ))
        )}
      </div>
    </div>
  );
}
