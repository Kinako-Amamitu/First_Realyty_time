@extends('layouts.app')
@section('title','フレンド一覧')
@section('h1','▼フレンド一覧▼')
@section('body')
    <form method="get" action="{{route('friendsfriendList')}}">
        @csrf
        <div class="search"><input type="search" id="search-text" name="id" placeholder="IDで検索">
            <button>検索</button>
        </div>
    </form>
    @if(!empty($user))
        <table class="table table-bordered mx-auto p-2" style="width: 60%">
            <tr>
                <th>ID</th>
                <th>ユーザー名</th>
                <th>フォローした人</th>
                <th>フォローチェック</th>
            </tr>
            @foreach ($user->follows as $users)
                <tr>
                    <td>{{$user->id}}</td>
                    <td>{{$user->player_name}}</td>
                    <td>{{$users->player_name}}</td>
                    <td>{{$users->pivot->follow_check}}</td>
                </tr>
            @endforeach
        </table>
    @endif
@endsection
