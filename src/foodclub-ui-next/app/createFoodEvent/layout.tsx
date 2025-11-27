"use client";

import { usePathname } from "next/navigation";
import FormWizardProvider, { useFormWizardContext } from "@/components/core/createFoodEventForms/formWizardContext";
import RoundedLink from "@/components/core/roundedLink";

function StepNavigation() {
  const pathname = usePathname();
  const [formState] = useFormWizardContext();
  
  // Extract current step number from pathname
  const currentStepMatch = pathname?.match(/\/step\/(\d+)/);
  const currentStep = currentStepMatch ? parseInt(currentStepMatch[1], 10) : 1;
  
  // Determine which steps are accessible (only current step and previous steps)
  const isStepAccessible = (stepNumber: number) => {
    // Always allow step 1
    if (stepNumber === 1) return true;
    
    // Only allow if it's the current step or a previous step (not future steps)
    return stepNumber <= currentStep;
  };

  return (
    <div className="flex gap-3">
      <RoundedLink href="/createFoodEvent/step/1" isDisabled={!isStepAccessible(1)}>1. Basics</RoundedLink>
      <RoundedLink href="/createFoodEvent/step/2" isDisabled={!isStepAccessible(2)}>2. When & Where</RoundedLink>
      <RoundedLink href="/createFoodEvent/step/3" isDisabled={!isStepAccessible(3)}>3. Pricing & Capacity</RoundedLink>
      <RoundedLink href="/createFoodEvent/step/4" isDisabled={!isStepAccessible(4)}>4. Diet & Allergens</RoundedLink>
      <RoundedLink href="/createFoodEvent/step/5" isDisabled={!isStepAccessible(5)}>5. Preview</RoundedLink>
    </div>
  );
}

export default function CreateFormLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {

  return (
    <div className="flex flex-col gap-5 p-5 ">
      <h2>Create a new event</h2>

      <FormWizardProvider>
        <StepNavigation />
        {children}
      </FormWizardProvider>

    </div>
  );
}