@extends('layouts.app')
@section('title','メール一覧')
@section('h1','▼ユーザー受信メール一覧▼')
@section('body')
    <table class="table table-bordered ">
        <form method="GET" action="{{route('mails.user')}}">
            @csrf
            <div class="search">
                <input type="search" name="id" placeholder="idを入力">
                <button>検索</button>
            </div>
        </form>
        @if(!empty($users))
            <tr>
                <th>ID</th>
                <th>プレイヤー名</th>
                <th>メールID</th>
                <th>受け取り</th>
            </tr>
            @foreach($users->mails as $user)
                <tr>

                    <td>{{$users['id']}}</td>
                    <td>{{$users->player_name}}</td>
                    <td>{{$user['mail_id']}}</td>
                    <td>{{$user['item_get']}}</td>

                </tr>
            @endforeach
        @endif
    </table>

@endsection
