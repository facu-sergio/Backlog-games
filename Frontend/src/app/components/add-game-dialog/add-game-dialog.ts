import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { GameInfo } from '../../core/interfaces/gameInfo.interface';

export interface AddGameDialogData {
  game: GameInfo;
  listName: string;
}

@Component({
  selector: 'app-add-game-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  templateUrl: './add-game-dialog.html',
  styleUrl: './add-game-dialog.scss'
})
export class AddGameDialogComponent {
  private dialogRef = inject(MatDialogRef<AddGameDialogComponent>);
  data: AddGameDialogData = inject(MAT_DIALOG_DATA);

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}
