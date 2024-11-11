@extends('layouts.app')
@section('title','アカウント登録')
@section('h1','▼アカウント登録▼')
@section('body')

    <form class="form-signin" method="POST" action="{{url('accounts/store')}}">
        @csrf
        <label for="inputEmail" class="sr-only">アカウント名</label>
        <input type="text" id="inputEmail" name="name" class="form-control" placeholder="アカウント名" required
               autofocus>
        <label for="inputPassword" class="sr-only">パスワード</label>
        <input type="password" id="inputPassword" name="password" class="form-control" placeholder="パスワード"
               required>
        <label for="inputPassword" class="sr-only">再入力</label>
        <input type="password" id="inputPassword_confirmation" name="password_confirmation" class="form-control"
               placeholder="パスワードの再入力" required>
        <div class="checkbox mb-3">
        </div>
        <button class="btn btn-lg btn-primary btn-block" name="login_btn" type="submit">アカウントを登録</button>
        <input type="hidden" name="action" value="doLogin">
        @if($errors->any())
            <ul>
                @foreach($errors->all() as $error)
                    <li>{{$error}}</li>
                @endforeach
            </ul>
        @endif
        <p class="mt-5 mb-3 text-muted">&copy; 2024</p>
    </form>
@endsection
