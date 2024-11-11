@extends('layouts.app')
@section('title','メール送信')
@section('h1','▼メール送信▼')
@section('body')
    <form method="POST" action="{{route('mails.register')}}">
        @csrf
        <label for="input_user_id" class="sr-only">ユーザーID</label>
        <input type="text" id="input_user_id" name="user_id" class="form-control" placeholder="ユーザーID"
               required>
        <label for="input_mail_id" class="sr-only">メールID</label>
        <input type="text" id="input_mail_id" name="mail_id" class="form-control" placeholder="メールID"
               required>
        <div class="checkbox mb-3">
        </div>
        <button class="btn btn-lg btn-primary btn-block" name="login_btn" type="submit">メールを送信</button>
        <input type="hidden" name="action" value="register">
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
