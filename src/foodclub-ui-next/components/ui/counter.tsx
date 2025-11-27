import * as React from 'react';
import { Button } from '@/components/ui/button';
import { Minus, Plus } from 'lucide-react';

type CounterProps = {
    value: number;
    onChange: (newValue: number) => void;
    min?: number;
    max?: number;
    step?: number;
};

export function Counter({ value, onChange, min = 0, max = Infinity, step = 1 }: CounterProps) {
    const handleDecrement = () => {
        const newValue = Math.max(min, value - step);
        onChange(newValue);
    };

    const handleIncrement = () => {
        const newValue = Math.min(max, value + step);
        onChange(newValue);
    };
    return (
        <div className="inline-flex items-center space-x-2">
            <Button type="button" variant="outline" onClick={handleDecrement} disabled={value <= min}>
                <Minus className="h-4 w-4" />
            </Button>
            <span className="text-sm font-medium">{value}</span>
            <Button type="button" variant="outline" onClick={handleIncrement} disabled={value >= max}>
                <Plus className="h-4 w-4" />
            </Button>
        </div>
    );
}