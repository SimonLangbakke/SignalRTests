"use client";

import { useState, useEffect } from "react";
import { Label } from "@/components/ui/label";
import { ImageDropzone } from "../imageDropzone";
import ComponentsWrapper from "../wrappers/componentsWrapper";
import CreateEventFormInput from "./createEventFormInput";
import CreateEventFormTextarea from "./createEventFormTextarea";
import MealCard from "../meal-card/meal-card";
import { EventOverviewDto } from "@/types/event-details.type";
import { FormBasics, FormWizardActionType, FormWizardStep, useFormWizardContext } from "./formWizardContext";
import { useRouter } from "next/navigation";
import { parseJwt } from "@/lib/jwt-decoder.helper";
import { createEventDefaultData } from "@/services/mocks/createEventDefaultData";


export default function BasicsForm() {


    const [formState, formDispatch] = useFormWizardContext();
    const router = useRouter();

    const [title, setTitle] = useState(formState.basics?.title ?? "");
    const [description, setDescription] = useState(formState.basics?.description ?? "");
    const [coverImage, setCoverImage] = useState(formState.basics?.coverImage ?? "");
    const [username, setUsername] = useState<string>("");

    useEffect(() => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            try {
                const decoded = parseJwt(token);
                setUsername(decoded.unique_name || "");
            } catch (err) {
                console.error("Failed to decode token:", err);
            }
        }
    }, []);

    let eventDetail: EventOverviewDto = {
        ...createEventDefaultData,
        name: title || createEventDefaultData.name,
        description: description || createEventDefaultData.description,
        imageThumbnail: coverImage || createEventDefaultData.imageThumbnail,
    };

    function onSubmit(e: React.FormEvent) {
        e.preventDefault();

        const formDataObj: FormBasics = {
            title: title,
            description: description,
            coverImage: coverImage,
        };

        formDispatch({ type: FormWizardActionType.Set, step: FormWizardStep.CreateFoodEvent, value: { basics: formDataObj } });

        router.push("/createFoodEvent/step/2");
    }


    return (
        // md:grid-cols-[2fr_1fr] 
        <div id="mainWrapper" className="grid grid-cols-1 md:grid-cols-[1fr_auto] gap-10">

            <form id="createFoodEventForm" onSubmit={onSubmit} >
                <ComponentsWrapper id="basicsForm">

                    <CreateEventFormInput
                        id="mealTitle"
                        labelText="Meal Title"
                        type="text"
                        placeholder="e.g., Cozy Curry Night"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />

                    <CreateEventFormTextarea
                        id="descriptionTextArea"
                        labelText="Description"
                        required
                        placeholder="Describe your menu, vibe, BYOB, etc."
                        className="h-48 bg-gray-50 text-sm font-normal text-[#9CA3AF] font-['Poppins'] resize-none"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                    />



                    <div className="grid gap-2">
                        <Label htmlFor="coverPhotoDropzone" className="text-xs text-muted-gray font-bold">Cover Photo</Label>
                        <ImageDropzone value={coverImage} onChange={setCoverImage} />
                    </div>


                    {/* <button type="submit" className="py-2 px-12 w-75 self-center border-1 cursor-pointer border-black text-white text-base font-bold font-['Poppins'] bg-primary-green rounded-xl hover:bg-muted hover:text-foreground transition-colors">Save & Continue</button> */}


                    {/* </div> */}
                </ComponentsWrapper>
            </form>


            <div className="flex flex-col">
                <ComponentsWrapper id="livePreview">
                    <h3>Live Preview</h3>
                    <MealCard key={123} {...eventDetail} name={title || eventDetail.name} hostName={username || eventDetail.hostName} />
                </ComponentsWrapper>

                <button type="submit" form="createFoodEventForm" className="py-2 px-12 w-75 self-center border-1 cursor-pointer border-black text-white text-base font-bold font-['Poppins'] bg-primary-green rounded-xl hover:bg-muted hover:text-foreground transition-colors">Save & Continue</button>
            </div>


        </div>
    );
}