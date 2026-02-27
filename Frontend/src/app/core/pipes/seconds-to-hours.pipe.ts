import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'secondsToHours',
  pure: true,
})
export class SecondsToHoursPipe implements PipeTransform {
  transform(seconds: number | null | undefined): string | null {
    if (seconds == null) return null;

    const hours = seconds / 3600;
    const floor = Math.floor(hours);
    const decimal = hours - floor;

    if (decimal >= 0.25 && decimal < 0.75) {
      return `${floor}½ Hrs`;
    }

    return `${Math.round(hours)} Hrs`;
  }
}
