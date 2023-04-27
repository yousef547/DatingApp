import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {
  @ViewChild('memberTabs',{static:true}) memberTabs:TabsetComponent;
member:Member;
galleryOptions: NgxGalleryOptions[];
galleryImages: NgxGalleryImage[];
activeTab:TabDirective;
messages:Message[] = [];
  constructor(private messageService:MessageService,private memberService:MembersService,private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.route.data.subscribe(data=>{
      this.member = data.member;
    })
    // this.loadMember();
    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab):this.selectTab(0);
    })

    this.galleryOptions = [{
      width:'500px',
      height:'500px',
      imagePercent:100,
      thumbnailsColumns:4,
      imageAnimation:NgxGalleryAnimation.Slide,
      preview:false
    }]
    this.galleryImages = this.getImages();
  }


  getImages(): NgxGalleryImage[]{
    const imageUrl= [];
    for(const photo of this.member.photos){
      imageUrl.push({
        small:photo?.url,
        medium:photo?.url,
        big:photo?.url,

      })
    }
    return imageUrl;
  }

  // loadMember(){
  //   this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
  //     this.member = member;
  //   })
  // }

  loadMessages(){
    this.messageService.getMessageThread(this.member.userName).subscribe(messages => {
      this.messages = messages;
    })
  }

  selectTab(tabId:number){
    this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(date:TabDirective){
    this.activeTab = date;
    if(this.activeTab.heading === 'Messages' && this.messages.length === 0){
      this.loadMessages();
    }
  }

}
