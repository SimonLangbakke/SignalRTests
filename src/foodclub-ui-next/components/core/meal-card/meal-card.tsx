import { EventOverviewDto } from "@/types/event-details.type";
import { Badge } from "../../ui/badge";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../../ui/card";
import Link from "next/link";
import "./meal-card.css";

export default function MealCard(eventDetails: EventOverviewDto) {
  // Truncate description to 3 lines (approximately 150 characters)
  const truncateDescription = (text: string | undefined, maxLength: number = 150) => {
    if (!text) return '';
    if (text.length <= maxLength) return text;
    return text.substring(0, maxLength).trim() + '...';
  };

  return (
    <Link href={`/viewEventPage?id=${eventDetails.eventId}`} className="block h-full">
      <Card className="h-full flex flex-col border-gray-200 shadow-sm hover:shadow-lg transition-all cursor-pointer overflow-hidden p-0">
        <div className="relative overflow-hidden h-48 flex-shrink-0">
          <img
            src={eventDetails.imageThumbnail}
            alt={eventDetails.name}
            className="w-full h-full object-cover transform hover:scale-105 transition-transform duration-300"
          />
          <div className="absolute top-2 right-2 bg-white/90 backdrop-blur-sm px-3 py-1 rounded-full shadow-md">
            <span className="font-bold text-emerald-800 text-sm">
              DKK {eventDetails.price}
            </span>
          </div>
          <div className="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-black/60 to-transparent p-4">
            <h3 className="text-white font-bold text-lg drop-shadow-lg">
              {eventDetails.name}
            </h3>
            <p className="text-white/90 text-sm drop-shadow">
              {eventDetails.eventAddress.city}
            </p>
          </div>
        </div>
        <CardHeader className="flex-grow p-4">
          <CardDescription className="text-md space-y-3">
            {eventDetails.description && (
              <div className="text-gray-700 line-clamp-3 text-sm">
                {truncateDescription(eventDetails.description)}
              </div>
            )}
            <div className="host-details text-sm text-gray-600">
              <div>Hosted by {eventDetails.hostName} · {eventDetails.hostRating} ★</div>
            </div>
            <div className="tags flex flex-wrap gap-1">
              {eventDetails.tags.slice(0, 3).map((tag) => (
                <Badge key={tag} variant="success" className="text-xs">
                  {tag}
                </Badge>
              ))}
              {eventDetails.tags.length > 3 && (
                <Badge variant="secondary" className="text-xs">+{eventDetails.tags.length - 3}</Badge>
              )}
            </div>
            {eventDetails.allergens && eventDetails.allergens.length > 0 && (
              <div className="allergens flex flex-wrap gap-1">
                {eventDetails.allergens.slice(0, 2).map((allergen) => (
                  <Badge key={allergen} variant="warning" className="bg-orange-100 text-orange-800 border-none text-xs">
                    {allergen}
                  </Badge>
                ))}
                {eventDetails.allergens.length > 2 && (
                  <Badge variant="warning" className="bg-orange-100 text-orange-800 border-none text-xs">
                    +{eventDetails.allergens.length - 2}
                  </Badge>
                )}
              </div>
            )}
            <div className="flex items-center justify-between text-sm pt-2 border-t">
              <span className="text-gray-600">
                {eventDetails.startDate} · {eventDetails.startTime}
              </span>
              <span className="font-semibold text-gray-700">
                {eventDetails.participantsCount}/{eventDetails.maxAllowedParticipants} seats
              </span>
            </div>
          </CardDescription>
        </CardHeader>
      </Card>
    </Link>
  );
}
