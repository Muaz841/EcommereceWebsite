import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { verificationComponent } from './verification.Component';
import { verificatioRoutingModulemodule } from './verification.RoutingModule';


@NgModule({
    declarations: [verificationComponent],
    imports: [verificatioRoutingModulemodule],
})

export class VerificationModule {}