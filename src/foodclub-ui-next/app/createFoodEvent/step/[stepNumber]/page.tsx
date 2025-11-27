import WhenWhereForm from "@/components/core/createFoodEventForms/whenWhereForm";
import BasicsForm from "@/components/core/createFoodEventForms/basicsForm";
import PriceCapacityForm from "@/components/core/createFoodEventForms/priceCapacityForm";
import DietAllergensForm from "@/components/core/createFoodEventForms/dietAllergensForm";
import PreviewForm from "@/components/core/createFoodEventForms/previewForm";
import { notFound } from "next/navigation";


const stepForms: React.ComponentType<any>[] = [BasicsForm, WhenWhereForm, PriceCapacityForm, DietAllergensForm, PreviewForm];

export default async function ReturnStepForm({ params }: { params: { stepNumber: string } }) {
    const {stepNumber} = await params;
    if (!Number.isInteger(Number(stepNumber))) {
        return notFound();
    }
    const idx = Number(stepNumber) - 1;
    if (idx < 0 || idx >= stepForms.length) {
        return notFound();
    }

    const Form = stepForms[idx];
    return <Form />;
}