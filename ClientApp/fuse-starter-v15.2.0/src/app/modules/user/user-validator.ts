import { AbstractControl, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { Observable } from "rxjs";
import { UserService } from "./user.service";

export class UserValidator {

   static emailValidator(userService: UserService): AsyncValidatorFn {
      return (control: AbstractControl): Observable<ValidationErrors | null> => {
         const val = userService.checkIfEmailExists(control.value);
         return val;
      };
   }
}