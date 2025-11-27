"use client";

import "./profilePage.css";
import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import HostedEvents from "./(tabs)/hosted-events/page";
import Reservations from "./(tabs)/reservations/page";
import Reviews from "./(tabs)/reviews/page";
import { getUserById } from "@/services/api/user-api.service";
import { parseJwt } from "@/lib/jwt-decoder.helper";
import { UserDetails } from "@/types/user-details.type";

type Tab = "hosted-events" | "reservations" | "reviews";

export default function UserProfile() {
  const [activeTab, setActiveTab] = useState<Tab>("hosted-events");
  const [userDetails, setUserDetails] = useState<UserDetails | null>(null);
  const router = useRouter();

  useEffect(() => {
    if (typeof window === "undefined") return;

    const token = localStorage.getItem("accessToken");
    if (!token) {
      router.replace("/");
      return;
    }

    try {
      const usertokenProps = parseJwt(token);
      (async () => {
        try {
          const user = await getUserById(usertokenProps.sub);
          setUserDetails(user);
        } catch (err) {
          console.error("Failed to fetch user details:", err);
        }
      })();
    } catch (err) {
      console.error("Invalid token:", err);
    }
  }, [router]);

  return (
    <div className="centered-profile-page">
      {/* User Info Section */}
      <div className="bg-white rounded-lg p-6 mb-8 flex items-start gap-6">
        <div className="relative">
          <div className="w-24 h-24 rounded-full bg-emerald-100 flex items-center justify-center text-2xl font-semibold">
            {userDetails?.name.substring(0, 1).toUpperCase()}{userDetails?.name.substring(1, 2).toUpperCase()}
          </div>
        </div>

        <div className="flex-1">
          <div className="flex justify-between items-start">
            <div>
              <h1 className="text-2xl font-bold mb-1">{userDetails?.name}</h1>
              <p className="text-gray-600 mb-2">address - TBD</p>
            </div>
          </div>

          {/* Navigation Tabs */}
          <div className="flex gap-4 mt-4">
            <button
              onClick={() => setActiveTab("hosted-events")}
              className={`px-4 py-2 rounded-full font-medium transition-colors ${
                activeTab === "hosted-events"
                  ? "bg-emerald-100 text-emerald-800"
                  : "text-gray-600 hover:bg-gray-50"
              }`}
            >
              Hosted Events
            </button>
            <button
              onClick={() => setActiveTab("reservations")}
              className={`px-4 py-2 rounded-full font-medium transition-colors ${
                activeTab === "reservations"
                  ? "bg-emerald-100 text-emerald-800"
                  : "text-gray-600 hover:bg-gray-50"
              }`}
            >
              My Reservations
            </button>
            <button
              onClick={() => setActiveTab("reviews")}
              className={`px-4 py-2 rounded-full font-medium transition-colors ${
                activeTab === "reviews"
                  ? "bg-emerald-100 text-emerald-800"
                  : "text-gray-600 hover:bg-gray-50"
              }`}
            >
              Reviews
            </button>
          </div>
        </div>
      </div>

      {/* Tab Content */}
      <div className="mt-8">
        {activeTab === "hosted-events" && <HostedEvents />}
        {activeTab === "reservations" && <Reservations />}
        {activeTab === "reviews" && <Reviews />}
      </div>
    </div>
  );
}
