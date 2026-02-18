import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { GameInfo } from '../../core/interfaces/gameInfo.interface';

export interface CompleteGameDialogData {
  game: GameInfo;
}

@Component({
  selector: 'app-complete-game-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule],
  templateUrl: './complete-game-dialog.html',
})
export class CompleteGameDialogComponent {
  private dialogRef = inject(MatDialogRef<CompleteGameDialogComponent>);
  data: CompleteGameDialogData = inject(MAT_DIALOG_DATA);

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}
