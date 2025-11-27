import { Counter } from "@/components/ui/counter"
import { Label } from "@/components/ui/label"
import * as React from "react"

interface CreateEventFormCounterProps {
  labelText: string
  id: string
  value: number
  onChange: (newValue: number) => void
  min?: number
  max?: number
  step?: number
}

export default function CreateEventFormCounter({
  labelText: label,
  id,
  value,
  onChange,
  min = 0,
  max = Infinity,
  step = 1,
}: CreateEventFormCounterProps) {
  return (
    <div className="grid gap-2">
      <Label
        htmlFor={id}
        className="text-xs font-bold text-muted-foreground"
      >
        {label}
      </Label>

      <Counter
        value={value}
        onChange={onChange}
        min={min}
        max={max}
        step={step}
      />
    </div>
  )
}
