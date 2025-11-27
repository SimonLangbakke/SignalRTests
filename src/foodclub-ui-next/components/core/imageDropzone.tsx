"use client"

import { useCallback } from "react"
import { useDropzone } from "react-dropzone"
import { Label } from "@/components/ui/label"

interface ImageDropzoneProps {
    value?: string;
    onChange?: (imageUrl: string) => void;
}

export function ImageDropzone({ value, onChange }: ImageDropzoneProps) {
    const onDrop = useCallback((acceptedFiles: File[]) => {
        if (acceptedFiles.length > 0 && onChange) {
            const file = acceptedFiles[0];
            const reader = new FileReader();
            reader.onloadend = () => {
                onChange(reader.result as string);
            };
            reader.readAsDataURL(file);
        }
    }, [onChange])

    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop,
        accept: { "image/*": [] },
    })

    return (

        <div
            {...getRootProps()}
            className={`flex h-32 w-80 items-center justify-center rounded-xl border border-dashed border-gray-300 bg-muted text-sm text-muted-foreground transition hover:bg-muted/80 ${isDragActive ? "border-primary bg-muted/50" : ""
                }`}
        >
            <input {...getInputProps()} />
            {value ? (
                <img src={value} alt="Preview" className="h-full w-full object-cover rounded-xl" />
            ) : (
                isDragActive ? "Drop the image here..." : "Drop image"
            )}
        </div>

    )
}
