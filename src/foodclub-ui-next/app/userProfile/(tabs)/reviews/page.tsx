"use client";

import { parseJwt } from "@/lib/jwt-decoder.helper";
import { getEventReviewsByHostUserId } from "@/services/api/events-api.service";
import { EventReviewDto } from "@/types/event-review.type";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Reviews() {
  const [userEventReviews, setUserEventReviews] = useState<
    Array<EventReviewDto>
  >([]);
  const router = useRouter();

  useEffect(() => {
    let isSubscribed = true;
    const controller = new AbortController();

    const fetchUserEventReviews = async () => {
      const token = localStorage.getItem("accessToken");
      if (!token) {
        router.replace("/");
        return;
      }

      try {
        const usertokenProps = parseJwt(token);
        const events = await getEventReviewsByHostUserId(
          usertokenProps.sub,
          token
        );
        if (isSubscribed) {
          setUserEventReviews(events);
        }
      } catch (err) {
        console.error("Failed to fetch event review details:", err);
        if (err instanceof Error && err.message.includes("Invalid token")) {
          router.replace("/");
        }
      }
    };

    fetchUserEventReviews();

    return () => {
      isSubscribed = false;
      controller.abort();
    };
  }, []);
  return (
    <div>
      <h2 className="text-xl font-bold mb-6">Reviews</h2>

      <div className="space-y-6">
        {userEventReviews.length === 0 ? (
          <p className="text-gray-500 text-center py-8">
            No event reviews found
          </p>
        ) : (
          userEventReviews.map((eventReview) => (
            <div key={eventReview.reviewId} className="bg-white rounded-lg p-6">
              <div className="flex items-center gap-4 mb-4">
                <div className="w-12 h-12 rounded-full bg-gray-100"></div>
                <div>
                  <h3 className="font-semibold">{eventReview.username}</h3>
                  <p className="text-gray-600 text-sm">
                    {eventReview.createdAt}
                  </p>
                </div>
              </div>
              <div className="flex gap-1 text-yellow-400 mb-2">
                {"★".repeat(eventReview.stars)}
              </div>
              <p className="text-gray-700">{eventReview.comment}</p>
            </div>
          ))
        )}
      </div>
    </div>
  );
}
