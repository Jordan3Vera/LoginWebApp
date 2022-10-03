import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Modules 
import { ButtonModule } from 'primeng/button';
import { TabMenuModule } from 'primeng/tabmenu';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { DialogModule } from 'primeng/dialog';
import { ToolbarModule } from 'primeng/toolbar';
import { CascadeSelectModule } from 'primeng/cascadeselect';
import { PanelModule } from 'primeng/panel';
import { CardModule } from 'primeng/card';
import { MenubarModule } from 'primeng/menubar';
import { ConfirmDialogModule } from 'primeng/confirmdialog'
import { TooltipModule } from 'primeng/tooltip';
import { SpeedDialModule } from 'primeng/speeddial';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { MegaMenuModule } from 'primeng/megamenu';
import { PanelMenuModule } from 'primeng/panelmenu';
import { MenuModule } from 'primeng/menu';
import { SplitButtonModule } from 'primeng/splitbutton';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import {CheckboxModule} from 'primeng/checkbox';
import {MessagesModule} from 'primeng/messages';
import {MessageModule} from 'primeng/message';
import { PasswordModule } from 'primeng/password';
import {KeyFilterModule} from 'primeng/keyfilter';
import {DividerModule} from 'primeng/divider';


@NgModule({
  imports: [
    CommonModule
  ],
  exports: [
    ButtonModule,
    TabMenuModule,
    InputTextModule,
    TableModule,
    DynamicDialogModule,
    DialogModule,
    ToolbarModule,
    CascadeSelectModule,
    PanelModule,
    CardModule,
    MenubarModule,
    ConfirmDialogModule,
    TooltipModule,
    SpeedDialModule,
    TieredMenuModule,
    MegaMenuModule,
    PanelMenuModule,
    MenuModule,
    SplitButtonModule,
    ProgressSpinnerModule,
    InputTextareaModule,
    DropdownModule,
    CardModule,
    CheckboxModule,
    MessageModule,
    MessagesModule,
    PasswordModule,
    KeyFilterModule,
    DividerModule
  ]
})
export class PrimengModule { }
