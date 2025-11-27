import { ActionDispatch, createContext, Dispatch, useContext, useReducer } from "react";

export type FormBasics = {
    title: string,
    description: string,
    coverImage?: string,
}

export type FormWhenWhere = {
    date: string,
    startTime: string,
    endTime?: string,
    
    streetAddress: string,
    postalCode: string,
    city: string,
    region: string
}

export type FormPriceCapacity = {
    seatsAvailable: number,
    pricePerSeat: number,
    guestsNotes?: string,
}

export type FormDietAllergens = {
    dietaryPreferences: string[],
    allergens: string[],
    guestsNotes?: string,
}

export type FormWizardState = {
    basics?: FormBasics,
    whenWhere?: FormWhenWhere,
    priceCapacity?: FormPriceCapacity,
    dietAllergens?: FormDietAllergens,
}

export enum FormWizardStep { CreateFoodEvent, WhenWhereForm, PriceCapacityForm, DietAllergensForm, PreviewForm }

export enum FormWizardActionType { Set }

export type FormWizardAction = {
    type: FormWizardActionType,
    step: FormWizardStep,
    value: FormWizardState
}

function formWizardReducer(currentValue: FormWizardState, action: FormWizardAction): FormWizardState {


    if (action.type === FormWizardActionType.Set) {
        switch (action.step) {
            case FormWizardStep.CreateFoodEvent:
                return { ...currentValue, basics: action.value.basics };
            case FormWizardStep.WhenWhereForm:
                return { ...currentValue, whenWhere: action.value.whenWhere };
            case FormWizardStep.PriceCapacityForm:
                return { ...currentValue, priceCapacity: action.value.priceCapacity };
            case FormWizardStep.DietAllergensForm:
                return { ...currentValue, dietAllergens: action.value.dietAllergens };
            case FormWizardStep.PreviewForm:
                throw new Error("Not implemented yet");
            //return { ...currentValue, preview: action.value.preview };
            default:
                throw new Error("Unknown FormWizardStep");
                break;
        }
    }

    throw new Error("Unknown FormWizardActionType");

}

const FormWizardContext = createContext<[FormWizardState | undefined, Dispatch<FormWizardAction> | undefined]>([undefined, undefined]);

export default function FormWizardProvider({ children }: { children: React.ReactNode }) {
    const reducerValue = useReducer(formWizardReducer, {});

    return <FormWizardContext.Provider value={reducerValue}>{children}</FormWizardContext.Provider>
}

export function useFormWizardContext(): [FormWizardState, Dispatch<FormWizardAction>] {
    const formContext = useContext(FormWizardContext);

    const [state, dispatch] = formContext;
    if (formContext === undefined || state === undefined || dispatch === undefined) {
        throw new Error("FormWizardContext is undefined.")
    }

    return [state, dispatch];
}