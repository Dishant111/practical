import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-cards-demo',
  imports: [CommonModule],
  templateUrl: './cards-demo.html',
  styleUrl: './cards-demo.scss',
})
export class CardsDemo {
  storageDrives: CloudeStorages[] = [
    {
      id: 1,
      name: 'Gogole Drive',
      icon: 'https://picsum.photos/200/300',
      description: 'Add google drtive account and manage your files from one place.',
    },
    {
      id: 2,
      name: 'One Drive',
      icon: 'https://picsum.photos/200/300',
      description: 'Add One drtive account and manage your files from one place.',
    },
    {
      id: 3,
      name: 'Mega Drive',
      icon: 'https://picsum.photos/200/300',
      description: 'Add mega drtive account and manage your files from one place.',
    },
  ];
}

export interface CloudeStorages {
  id: number;
  name: string;
  icon: string;
  description: string;
}
