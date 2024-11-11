@extends('layouts.app')
@section('title','アカウント更新')
@section('h1','▼アカウント更新▼')
@section('body')
    @if($errors->any())
        <ul>
            @foreach($errors->all() as $error)
                <li>{{$error}}</li>
            @endforeach
        </ul>
    @endif
    <form method="post" action="{{route('accounts.update')}}">
        @csrf
        <h1>パスワード更新：{{$account['name']}}</h1>
        <label for="inputPassword" class="sr-only">新しいパスワード</label>
        <input type="password" id="inputPassword" name="password" class="form-control" placeholder="パスワード"
               required>
        <label for="inputPassword" class="sr-only">再入力</label>
        <input type="password" id="inputPassword_confirmation" name="password_confirmation" class="form-control"
               placeholder="パスワードの再入力"
               required>
        <button type="submit" class="btn btn-secondary">更新</button>
        <input type="hidden" value="update">
        <input type="hidden" name='id' value={{$account['id']}} >
        <button onclick="location.href='{{route('accounts.index')}}'" type="button" class="btn btn-secondary">
            キャンセル
        </button>
    </form>
@endsection
