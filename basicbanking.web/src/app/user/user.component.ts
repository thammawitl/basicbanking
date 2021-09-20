import { Component, OnInit } from '@angular/core';
import { BankingResource } from '../common/resource/banking.service'
import Swal from 'sweetalert2'

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})

export class UserComponent implements OnInit {
  constructor(
    private bankingResource: BankingResource,
  ) { }

  ngOnInit(): void {
    this.simpleAlert()
  }

  simpleAlert(){  
    Swal.fire({
      title: 'Loading Data',
      showCancelButton: false,
      showLoaderOnConfirm: true,
      didOpen: () => {
        return this.bankingResource.getUserList()
          .subscribe((data: Users) => {
            return response.json()
          },
          error => {
            Swal.fire({
              icon: 'error',
              timer: 1500,
              timerProgressBar: true,
              title: error,
              text: 'Something went wrong try refresh the page!',
            })
          }
          )
      },
      allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire({
          title: `${result.value.login}'s avatar`,
          imageUrl: result.value.avatar_url
        })
      }
    })
  }  

}
