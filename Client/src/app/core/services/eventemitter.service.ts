import { EventEmitter, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventEmitterService {
    event: EventEmitter<any> = new EventEmitter();
    noti: EventEmitter<any> = new EventEmitter();
    omicall: EventEmitter<any> = new EventEmitter();
    chat: EventEmitter<any> = new EventEmitter();
    updateCountIconMessageChat: EventEmitter<any> = new EventEmitter();
    // constructor() { }
    // emitEvent(number) {
    //     this.event.emit(number);
    // }
    // subscribeEvent() {
    //     return this.event;
    // }
}