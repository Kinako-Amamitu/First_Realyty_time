@extends('layouts.app')
@section('title','マスターデータ一覧')
@section('h1','▼マスターデータ一覧▼')
@section('body')

    <table class="table table-bordered mx-auto p-2" style="width: 60%">
        @if(!empty($mails))
            <tr>
                <th>ID</th>
                <th>本文</th>
                <th>アイテム</th>
            </tr>
            @foreach ($mails as $mail)
                <tr>
                    <td>{{$mail['id']}}</td>
                    <td>{{$mail['Text']}}</td>
                    <td>{{$mail['item_name']}}</td>

                </tr>
            @endforeach
    </table>
    @endif
    <form method="get" action="{{route('mails.send')}}">
        @csrf
        <button type="submit" class="btn btn-secondary">メールを送信する</button>
        <input type="hidden" value="delete">
    </form>
@endsection

