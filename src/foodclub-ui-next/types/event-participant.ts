import { PaymentStatus } from "@/types/payment-status";
import { ParticipantStatus } from "@/types/participant-status";

export type EventParticipantDto = {
    id: string;
    eventId: string;
    userId: string;
    createdAt: string;
    participantStatus: ParticipantStatus;
    paymentStatus: PaymentStatus;
    paymentIntentId: string;
}